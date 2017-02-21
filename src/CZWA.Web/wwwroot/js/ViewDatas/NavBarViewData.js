
window.ViewModels = (function (module) {
    module.NavBarViewData = function (data) {
        var self = this;
        ko.mapping.fromJS(data, {}, self);
    };
    return module;
}(this.ViewModels || {}));