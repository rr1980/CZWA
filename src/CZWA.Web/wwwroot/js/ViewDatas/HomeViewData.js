
var HomeViewData = function (data) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);

    //var newData = ko.mapping.toJS(viewModel);
};