function AlertUser() {
    document.getElementById("msgDiv").innerHTML = MessageUser("Shaikh");
}

function MessageUser(user: string) {
    return "<h2>Hello " + user + ", your order was registered!</h2>";
}
