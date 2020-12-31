function changeStatus(title, msg, url, newStatus)
{
    var statusChange = (newStatus == true ? false : true);
    var statusChangeMsg = (newStatus == true ? 'activate' : 'deactivate');

    swal({
        title: title,
        text: "Do you want to " + statusChangeMsg + " this " + msg + "?",
        icon: "warning",
        dangerMode: statusChange,
        buttons: true
    }).then((willChange) =>
    {
        if (willChange)
        {
            $.ajax({
                type: "PUT",
                url: url,
                success: function (data)
                {
                    if (data.success)
                    {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else
                    {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}