require.config({
    baseUrl: "/Scripts/Albingia/es5",
    paths: {
        jquery: "../../Scripts/Jquery/jquery-1.8.2",
        jqueryValidate: "../../Scripts/Jquery/jquery.validate.min",
        jqueryValidateUnobtrusive: "../../Scripts/Jquery/jquery.validate.unobtrusive.min",
        knockout: "../../Scripts/knockout/3.5/knockout",
        knockoutMapping: "../../Scripts/knockout/knockout.mapping-latest.debug"
    },
    shim: {
        jqueryValidate: ["jquery"],
        jqueryValidateUnobtrusive: ["jquery", "jqueryValidate"]
    }
});
