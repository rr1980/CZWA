

var adminViewModelTestData = {
    users: [
        {
            userId: 1,
            name: "Riesner",
            vorname: "Rene",
            username: "rr1980",
            showname: "rr1980",
            showName: "rr1980",
            roles: [
                0, 1
            ]
        },
        {
            userId: 1,
            name: "Müller",
            vorname: "Klaus",
            username: "Oxi",
            showname: "Oxi",
            roles: [
                1
            ]
        }
    ],
    erros: []
};


var vm;

module("CZWA.AdminViewData");
test("init...", function () {
    vm = new ViewModels.AdminViewData(adminViewModelTestData);
    ok(vm, "vm is null");
});

test("User...", function () {
    var user = vm.user();
    ok(user, "user is null");

    equal(user.userId(), 1, "user.userId wrong");
    equal(user.name(), "Riesner", "user.name wrong");
    equal(user.vorname(), "Rene", "user.vorname wrong");
    equal(user.username(), "rr1980", "user.username wrong");
    equal(user.showName(), "rr1980", "user.showName wrong");
    equal(user.roles().length, 2, "user.roles wrong count");
    equal(user.roles()[0], 0, "user.roles wrong id");
    equal(user.roles()[1], 1, "user.roles wrong id");
});