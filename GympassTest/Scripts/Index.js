$("form").submit(function (e) {
    e.preventDefault();
    var formdata = new FormData();
    if ($("#file").prop("files").length > 0) {
        formdata.append("file", $("#file").prop("files")[0]);
    }

    $.ajax({
        url: "/Home/Submit",
        type: "POST",
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {
            $("#classificacao").html(result);
        }
    });
});