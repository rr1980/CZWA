
window.ViewModels = (function (module) {
    module.AdminViewData = function (data) {
        var self = this;
        ko.mapping.fromJS(data, {}, self);
        var adminService = new Services.AdminService();

        self.selectedUserId = ko.observable();
        self.user = ko.observable(self.users()[0]);


        self.onClickDelete = function () {
            adminService.delUser(ko.mapping.toJS(self.user)).done(function (response) {

            });
        };

        self.onClickSave = function () {
            var userna = self.user().username();
            adminService.saveUser(ko.mapping.toJS(self.user)).done(function (response) {
                if (response.errors === null) {
                    ko.mapping.fromJS(response.users, {}, self.users);
                    for (var i = 0; i < self.users().length; i++) {
                        if (self.users()[i].username() === userna) {
                            self.selectedUserId(self.users()[i].userId());
                            break;
                        }
                    }
                    $(".selectpicker").selectpicker('refresh');
                }
                else {
                    $("#errors").html("");

                    for (var j = 0; j < response.errors.length; ji++) {
                        $("#errors").append("<li style='color:red;'>" + response.errors[j] + "</li>");
                    }
                }
            });
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