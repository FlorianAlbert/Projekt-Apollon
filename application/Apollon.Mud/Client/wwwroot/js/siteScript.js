function goToBottom(id) {
    var element = document.getElementById(id);
    element.scrollTop = element.scrollHeight - element.clientHeight;
}

function clearPrompt(id) {
    document.getElementById(id).value = '';
}