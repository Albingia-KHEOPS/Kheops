
var FrDate = function (d, m, y) {
    this._isValid = null;
    if (typeof d === "string" && !m && !y) {
        var dtMatches = s.match(/^([0-3]?\d)\/([0-1]?\d)\/(\d{4})$/);
        if (dtMatches) {
            this.day = parseInt(dtMatches[1]);
            this.month = parseInt(dtMatches[2]);
            this.year = parseInt(dtMatches[3]);
            this.hours = 0;
            this.minutes = 0;
            this.seconds = 0;
        }
        else if (dtMatches = s.match(/^([0-3]?\d)\/([0-1]?\d)\/(\d{4})\s([0-2]\d):([0-5]\d)(?::([0-5]\d))?$/)) {
            this.day = parseInt(dtMatches[1]);
            this.month = parseInt(dtMatches[2]);
            this.year = parseInt(dtMatches[3]);
            this.hours = parseInt(dtMatches[4]);
            this.minutes = parseInt(dtMatches[5]);
            this.seconds = dtMatches[6] ? parseInt(dtMatches[6]) : 0;
        }
    }
    else {
        this.day = parseInt(d);
        this.month = parseInt(m);
        this.year = parseInt(y);
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

FrDate.prototype.toTimeString = function () {
    return this.hours.toString().padStart(2, "0")
        + ":" + this.minutes.toString().padStart(2, "0")
        + (isNaN(this.seconds) ? "" : ":" + this.seconds.toString().padStart(2, "0"));
};

FrDate.prototype.toJsDate = function () {
    return this.isValid ? new Date(this.year, this.month, this.day, this.hours, this.minutes, this.seconds) : null;
};

FrDate.checkString = function (s) {
    if (typeof s !== "string") return false;
    var dtMatches = s.match(/^([0-3]?\d)\/([0-1]?\d)\/(\d{4})$/);
    if (!dtMatches) {
        return false;
    }

    return FrDate.check(dtMatches[0], dtMatches[1], dtMatches[2]);
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

    if (isNaN(date.hour) || isNaN(date.minutes) || isNaN(date.seconds)) {
        return false;
    }

    if (date.hour > 23 || date.hour < 0 || date.minutes > 59 || date.minutes < 0 || date.seconds > 59 || date.seconds < 0) {
        return false;
    }

    return true;
};

FrDate.compare = function (date1, date2) {
    var d1 = null;
    var d2 = null;
    if (date1 instanceof FrDate && date2 instanceof FrDate) {
        d1 = date1.toJsDate();
        d2 = date2.toJsDate();
    }
    else if (typeof date1 === "string" && typeof date2 === "string") {
        var fd1 = new FrDate(date1);
        var fd2 = new FrDate(date2);
        d1 = fd1.toJsDate();
        d2 = fd2.toJsDate();
    }

    if (d1 && d2) {
        return d1 == d2 ? 0 : d1 < d2 ? -1 : 1;
    }

    return null;
};

Object.defineProperty(FrDate.prototype, "isValid", {
    enumerable: true,
    configurable: false,
    writable: false,
    get: function () {
        this._isValid = FrDate.check(this);
        return this._isValid;
    }
});

