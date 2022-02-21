function modalMessage(pageURL, id) {
    $("#Dialog").load(pageURL + "?id=" + id);
}