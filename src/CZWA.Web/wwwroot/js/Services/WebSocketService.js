window.Services = (function (module) {
    module.WebSocketService = function () {
        var service = {};
        var serviceurl = "http://" + window.location.host + "/WebSocket/";
        var complete = function () {
            
        };
        var start = function (message) {
            
        };

        service.sendmessage = function (message) {
            start();
            //console.debug(mainItem);
            return $.ajax({
                url: serviceurl + "sendmessage",
                data: {
                    message: message
                },
                type: "Get",
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