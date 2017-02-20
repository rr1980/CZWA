// Write your Javascript code.
var list = document.getElementById("messages");
var button = document.getElementById("sendButton");

var connection = new WebSocketManager.Connection("ws://localhost:63497/notifications");

connection.clientMethods["receiveMessage"] = (message) => {
    var messageText = "Someone said: " + message;

    console.log(messageText);
    appendItem(list, messageText);
};
connection.start();

function sendMessage(message) {
    console.log("Sending through HTTP to a controller:" + message);

    $.ajax({
        url: "http://" + window.location.host + "/WebSocket/sendmessage?message=" + message,
        method: 'GET'
    });
}
function appendItem(list, message) {
    var item = document.createElement("li");
    item.appendChild(document.createTextNode(message));
    list.appendChild(item);
}

$(document).ready(function () {

    button.addEventListener("click", function () {

        var input = document.getElementById("textInput");
        sendMessage(input.value);

        input.value = "";
    });
});