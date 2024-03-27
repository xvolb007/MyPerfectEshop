$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#tblData').DataTable({
        "ajax": {
            "url": '/admin/product/getall'
        },
        "columns": [
            { "data": "title", "width": "15%" }, // Свойство "title" вместо "Title"
            { "data": "isbn", "width": "15%" }, // Свойство "isbn"
            { "data": "price", "width": "15%" }, // Свойство "price"
            { "data": "author", "width": "15%" }, // Свойство "author"
            { "data": "category.name", "width": "15%" } // Для доступа к вложенным свойствам используйте точечную нотацию
        ]
    });
}
function Delete(url){
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
