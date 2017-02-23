
window.ViewModels = (function (module) {
    module.NavBarViewData = function (data) {
        var self = this;
        ko.mapping.fromJS(data, {}, self);

        self.onClickLogout = function () {
            connection.socket.close();
            window.location.href = "Account/Logout";
        };

    };
    return module;
}(this.ViewModels || {}));