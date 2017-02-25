
window.ViewModels = (function (module) {
    module.HomeViewData = function (data) {
        var self = this;
        self.tinput = ko.observable("");
        self.msgs = ko.observableArray();
        var wService = new Services.WebSocketService();

        ko.mapping.fromJS(data, {}, self);


        connection.clientMethods["receiveMessage"] = (message) => {
            var messageText = "Someone said: " + message;

            console.log(messageText);

            self.msgs.push(messageText);
        };


        self.onClickSend = function () {
            console.log("Sending through HTTP to a controller:" + self.tinput());

            //wService.sendmessage(self.tinput()).done(function (response) {
            //    console.debug(response);
            //});

            connection.invoke("TestMethode", "Riesner");

            console.debug(getCookie("rrAuth"));
        };
    };
    return module;
}(this.ViewModels || {}));