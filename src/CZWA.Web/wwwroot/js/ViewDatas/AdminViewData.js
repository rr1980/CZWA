
window.ViewModels = (function (module) {
    module.AdminViewData = function (data) {
        var self = this;
        ko.mapping.fromJS(data, {}, self);

        self.selectedUserId = ko.observable();
        self.user = ko.observable(self.users()[0]);

        self.onClickInsert = function () {
            console.debug(self.user());
        };

        self.onClickEdit = function () {
            console.debug("onClickEdit");
        };

        self.onClickDelete = function () {
            console.debug("onClickDelete");
        };

        self.onClickSave = function () {
            console.debug("onClickSave");
        };


        self.selectedUserId.subscribe(function () {
            for (var i = 0; i < self.users().length; i++) {
                if (self.selectedUserId() === self.users()[i].userId()) {
                    self.user(self.users()[i]);
                    break;
                }
            }
            $(".selectpicker").selectpicker('refresh');
        });

    };
    return module;
}(this.ViewModels || {}));