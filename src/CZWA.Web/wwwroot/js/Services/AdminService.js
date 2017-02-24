window.Services = (function (module) {
    module.AdminService = function () {
        var service = {};
        var serviceurl = "http://" + window.location.host + "/Admin/";
        var complete = function () {
            
        };
        var start = function (message) {
            
        };

        service.saveUser = function (user) {
            start();
            //console.debug(mainItem);
            return $.ajax({
                url: serviceurl + "SaveUser",
                data: {
                    user: user
                },
                type: "POST",
                cache: false
            }).fail(function (a, b, c) {
                console.debug(a);
                console.debug(b);
                console.debug(c);
            }).always(complete);
        };

        service.delUser = function (user) {
            start();
            //console.debug(mainItem);
            return $.ajax({
                url: serviceurl + "DelUser",
                data: {
                    user: user
                },
                type: "POST",
                cache: false
            }).fail(function (a, b, c) {
                console.debug(a);
                console.debug(b);
                console.debug(c);
            }).always(complete);
        };

        return service;
    };
    return module;
}(this.Services || {}));