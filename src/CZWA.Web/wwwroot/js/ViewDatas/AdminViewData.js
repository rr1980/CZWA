
window.ViewModels = (function (module) {
    module.AdminViewData = function (data) {
        var self = this;
        self.selectedRoleId = ko.observableArray([2]);
        ko.mapping.fromJS(data, {}, self);



        self.user = ko.computed({
            read: function () {
                var result;
                for (var i = 0; i < self.users().length; i++) {
                    var id = self.users()[i].userId();
                    if (self.selectedUserId() === self.users()[i].userId()) {
                        result = self.users()[i];
                    }
                }

                return result;
            },
            write: function (value) {
                
            },
            owner: this
        });
        $(".selectpicker").selectpicker('refresh');
    };
    return module;
}(this.ViewModels || {}));