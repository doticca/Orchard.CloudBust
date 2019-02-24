$(function () {
    $('#FileName').on('change', function () { fileChanged(); });

    var file = $('#FileName').val();
    $('#deleteLink').attr('href', deleteUrl.substring(0, deleteUrl.length - 1) + file);
    $('#saveLink').attr('href', saveUrl.substring(0, saveUrl.length - 1) + file);
});

function fileChanged() {
    var file = $('#FileName').val();
    $('#deleteLink').attr('href', deleteUrl.substring(0, deleteUrl.length - 1) + file);
    $('#saveLink').attr('href', saveUrl.substring(0, saveUrl.length - 1) + file);

    $.ajax({
        data: { 'file': file },
        url: indexUrl,
        cache: false,
        success: function (data) {
            $('#Content').val(data);
        },
        error: function () { alert("Could not load data!"); }
    });
}