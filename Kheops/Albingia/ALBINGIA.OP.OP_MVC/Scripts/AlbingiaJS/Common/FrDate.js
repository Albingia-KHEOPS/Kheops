
var FrDate = function (d, m, y) {
    this._isValid = null;
    if (!m && !y) {
        if (typeof d === "string") {
            var dtMatches = d.match(/^([0-3]?\d)\/([0-1]?\d)\/(\d{4})$/);
            if (dtMatches) {
                this.day = parseInt(dtMatches[1]);
                this.month = parseInt(dtMatches[2]);
                this.year = parseInt(dtMatches[3]);
                this.hours = 0;
                this.minutes = 0;
                this.seconds = 0;
            }
            else if ((dtMatches = d.match(/^([0-3]?\d)\/([0-1]?\d)\/(\d{4})\s([0-2]\d):([0-5]\d)(?::([0-5]\d))?$/))) {
                this.day = parseInt(dtMatches[1]);
                this.month = parseInt(dtMatches[2]);
                this.year = parseInt(dtMatches[3]);
                this.hours = parseInt(dtMatches[4]);
                this.minutes = parseInt(dtMatches[5]);
                this.seconds = dtMatches[6] ? parseInt(dtMatches[6]) : 0;
            }
        }
        else if (d instanceof Date) {
            this.day = d.getDate();
            this.month = d.getMonth() + 1;
            this.year = d.getFullYear();
            this.hours = d.getHours();
            this.minutes = d.getMinutes();
            this.seconds = d.getSeconds();
        }
    }
    else {
        var i = 0;
        this.day = isNaN(i = parseInt(d)) ? null : i;
        this.month = isNaN(i = parseInt(m)) ? null : i;
        this.year = isNaN(i = parseInt(y)) ? null : i;
        this.hours = 0;
        this.minutes = 0;
        this.seconds = 0;
    }
};

FrDate.prototype.toDateString = function () {
    if (!this.isValid) {
        return "";
    }

    return this.day.toString().padStart(2, "0") + "/" + this.month.toString().padStart(2, "0") + "/" + this.year.toString();
};

FrDate.prototype.toTimeString = function (includeSeconds) {
    return this.hours.toString().padStart(2, "0")
        + ":" + this.minutes.toString().padStart(2, "0")
        + (isNaN(this.seconds) || !includeSeconds ? "" : ":" + this.seconds.toString().padStart(2, "0"));
};

FrDate.prototype.changeDate = function (dateString) {
    if (typeof dateString !== "string") return false;
    var dtMatches = dateString.match(/^([0-3]?\d)\/([0-1]?\d)\/(\d{4})$/);
    if (!dtMatches) {
        return false;
    }

    this.day = parseInt(dtMatches[1]);
    this.month = parseInt(dtMatches[2]);
    this.year = parseInt(dtMatches[3]);
    return FrDate.check(this);
};

FrDate.prototype.changeTime = function (timeString) {
    if (typeof timeString !== "string") return false;
    var dtMatches = timeString.match(/^([0-2]\d):([0-5]\d)(?::([0-5]\d))?$/);
    if (!dtMatches) {
        return false;
    }

    this.hours = parseInt(dtMatches[1]);
    this.minutes = parseInt(dtMatches[2]);
    this.seconds = parseInt(dtMatches[3]);
    return FrDate.check(this);
};

FrDate.prototype.toJsDate = function () {
    return this.isValid ? new Date(this.year, this.month - 1, this.day, this.hours, this.minutes, this.seconds) : null;
};

if (ko) {
    FrDate.ko = {
        update: function (dt, value, property) {
            if (!dt || !ko.isObservable(dt)) return;
            var date = null;
            if (ko.mapping && ko.mapping.isMapped(dt)) {
                date = ko.mapping.toJS(dt);
            }
            else {
                date = ko.toJS(dt);
            }
            if (!date) {
                date = new FrDate();
            }
            else if (!FrDate.resemble(date)) {
                return null;
            }
            var pp = null;
            for (var p in date) {
                if (!property || p == property) {
                    pp = property;
                    break;
                }
            }
            if (pp != null) {
                if (pp == "") {
                    if (!value && value != 0) {
                        dt(value);
                    }
                    else {
                        dt(new FrDate(value));
                    }
                }
                else {
                    date[pp] = value;
                    dt(date);
                }
            }
            
            return dt;
        },
        createGetSetWithHour: function (koObj, propname, koHours, koMinutes, createFrDate, initialDate, frDateName, preventSet) {
            let observableField = null;
            let dt = initialDate;
            if (ko.isObservable(initialDate)) {
                observableField = initialDate;
                dt = observableField();
            }
            if (createFrDate) {
                frDateName = frDateName || "fr_" + propname;
                koObj[frDateName] = ko.observable(dt ? new FrDate(dt) : null);
            }
            else if (!frDateName) {
                throw "A valid name of the FrDate property has to be supplied";
            }
            let frDate = koObj[frDateName];
            koObj[propname] = ko.pureComputed({
                read: function () {
                    return frDate() ? frDate().toDateString() : "";
                },
                write: function (value) {
                    if (typeof preventSet === "function" && preventSet(value, propname)) {
                        // notify subscribers in ordre to trigger the read and reset bound(input) value
                        ko.tasks.schedule(function () {
                            koObj[propname].notifySubscribers(frDate() ? frDate().toDateString() : "");
                        });
                        return false;
                    }
                    if (!value) {
                        frDate(null);
                    }
                    else {
                        if (ko.isObservable(koHours) && ko.isObservable(koMinutes)) {
                            frDate(new FrDate(value + " " + (koHours() || "00") + ":" + (koMinutes() || "00")));
                        }
                        else {
                            frDate(new FrDate(value));
                        }
                    }
                    if (ko.isObservable(observableField)) {
                        observableField(frDate() ? frDate().toDateString() : null);
                    }
                }
            });

            if (ko.isObservable(observableField)) {
                observableField.subscribe(function (value) {
                    frDate(value ? new FrDate(value) : null);
                });
            }
        },
        createGetSet: function (koObj, propname, createFrDate, initialDate, frDateName, preventSet) {
            FrDate.ko.createGetSetWithHour(koObj, propname, null, null, createFrDate, initialDate, frDateName, preventSet);
        }
    };
}

FrDate.checkString = function (s) {
    if (typeof s !== "string") return false;
    var dtMatches = s.match(/^([0-3]?\d)\/([0-1]?\d)\/(\d{4})$/);
    if (!dtMatches) {
        return false;
    }

    return FrDate.check(dtMatches[1], dtMatches[2], dtMatches[3]);
};

FrDate.check = function (date) {
    if (!(date instanceof FrDate)) {
        return null;
    }

    if (isNaN(date.year) || isNaN(date.month) || isNaN(date.day) || date.year < 1 || date.month > 12 || date.month < 1 || date.day < 1 || date.day > 31) {
        return false;
    }

    var daysInMonth = new Date(date.year, date.month, 0).getDate();
    if (date.day > daysInMonth) {
        return false;
    }

    if (date.hours && isNaN(date.hours) || date.minutes && isNaN(date.minutes) || date.seconds && isNaN(date.seconds)) {
        return false;
    }

    if (date.hours > 23 || date.hours < 0 || date.minutes > 59 || date.minutes < 0 || date.seconds > 59 || date.seconds < 0) {
        return false;
    }

    return true;
};

FrDate.compare = function (date1, date2) {
    let d1 = null;
    let d2 = null;
    if (date1 instanceof FrDate && date2 instanceof FrDate) {
        d1 = date1.toJsDate();
        d2 = date2.toJsDate();
    }
    else if (typeof date1 === "string" && typeof date2 === "string") {
        let fd1 = new FrDate(date1);
        let fd2 = new FrDate(date2);
        d1 = fd1.toJsDate();
        d2 = fd2.toJsDate();
    }

    if (d1 && d2) {
        return d1 == d2 ? 0 : d1 < d2 ? -1 : 1;
    }

    return null;
};

FrDate.today = function () {
    var jsDate = new Date();
    return new FrDate(jsDate.getDate(), jsDate.getMonth() + 1, jsDate.getFullYear());
};

FrDate.resemble = function (obj) {
    if (!obj) return false;
    var d = new FrDate();
    var basicProperties = [];
    for (var p in d) {
        if (typeof d[p] !== "function" && p[0] != "_") {
            basicProperties.push(p);
        }
    }
    var index;
    for (var p in obj) {
        index = basicProperties.indexOf(p);
        if (index > -1) {
            basicProperties.splice(index, 1);
        }
    }

    return basicProperties.length == 0;
};

Object.defineProperty(FrDate.prototype, "isValid", {
    enumerable: true,
    configurable: false,
    get: function () {
        this._isValid = FrDate.check(this);
        return this._isValid;
    }
});

