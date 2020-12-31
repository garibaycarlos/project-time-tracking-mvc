var dataTable;

$(document).ready(function ()
{
    loadDataTable();
});

function loadDataTable()
{
    dataTable = $('#DT_load').DataTable({
        ajax: {
            url: "/user/home/getall",
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: "category.name", width: "10%" },
            { data: "customer.name", width: "10%" },
            {
                data: "",
                render: function (data, type, row)
                {
                    var fullName = row.resource.firstName + ' ' + row.resource.lastName

                    return fullName;
                },
                width: "10%"
            },
            { data: "projectStatus.name", width: "10%" },
            { data: "initiator", width: "12%" },
            { data: "description", width: "15%" },
            { data: "hours" },
            { data: "creationDate", width: "11%" },
            { data: "startDate", width: "11%" },
            { data: "completionDate", width: "11%" },
            {
                data: "id",
                render: function (data)
                {
                    var editAction = `/User/Home/Upsert?id=${data}`;
                    var deleteAction = `deleteRecord('Project Track - Project Listing', 'Are you sure you want to delete the project?', '/User/Home/Delete?id=${data}');`;

                    return `<div class="text-center">
                                <a href="${editAction}" style="color: #FFC107; text-decoration: none;" title="Edit">
                                    <i class="fas fa-pen"></i>
                                </a>
                                &nbsp;
                                <a style="color: #E34724; cursor:pointer;" title="Delete" onclick="${deleteAction}">
                                    <i class="fas fa-trash"></i>
                                </a>
                            </div>`;
                },
                orderable: false
            }
        ],
        language: {
            emptyTable: "No data to display"
        },
        width: "100%",
        columnDefs: [
            {
                targets: [7, 8, 9], render: function (data)
                {
                    if (data) // there is a vallid date
                    {
                        return moment(data).format('MMMM Do YYYY');
                    }
                    else
                    {
                        return '';
                    }
                }
            },
            {
                targets: [0, 1, 2, 4], className: "truncateStandard"
            },
            {
                targets: 5, className: "truncateDescription"
            }
        ],
        createdRow: function (row)
        {
            $(row).find(".truncateStandard").each(function ()
            {
                $(this).attr("title", this.innerText);
            });

            $(row).find(".truncateDescription").each(function ()
            {
                $(this).attr("title", this.innerText);
            });
        }
    });
}