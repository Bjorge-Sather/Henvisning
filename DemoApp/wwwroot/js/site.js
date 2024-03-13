function postForm(path) {
    document.forms[0].action = path;
    document.forms[0].submit();
}