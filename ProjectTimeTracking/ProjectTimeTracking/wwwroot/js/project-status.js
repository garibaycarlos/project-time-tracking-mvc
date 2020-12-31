﻿var dataTable;

$(document).ready(function ()
{
    loadDataTable();
});

function loadDataTable()
{
    dataTable = $('#DT_load').DataTable({
        ajax: {
            url: "/admin/projectStatus/getall",
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: "name", width: "35%" },
            { data: "description", width: "65%" },
            {
                data: "isActive",
                render: function (data, type, row)
                {
                    var statusColor = (data == true ? '#27C46B' : '#E34724');
                    var statusTitle = (data == true ? 'Deactivate' : 'Activate');
                    var statusClass = (data == true ? 'fas fa-user-check' : 'fas fa-user-times');
                    var statusChangeTo = (data == true ? false : true);

                    return `<div class="text-center">
                                <a style="color: ${statusColor}; cursor:pointer;" title="${statusTitle} project status" onclick="changeStatus('Project Track - Project Status', 'project status', '/Admin/ProjectStatus/ChangeStatus?id=${row.id}&isActive=${statusChangeTo}', ${statusChangeTo})">
                                    <i class="${statusClass}"></i>
                                </a>
                            </div>`;
                },
                orderable: false
            },
            {
                data: "id",
                render: function (data, type, row)
                {
                    var editTitle = (row.isActive == true ? "Edit" : "Cannot edit an inactive project status");
                    var editIconColor = (row.isActive == true ? "#FFC107" : "gray");
                    var editAction = (row.isActive == true ? `/Admin/ProjectStatus/Upsert?id=${data}` : "#");
                    var deleteTitle = (row.isActive == false ? "Delete" : "Cannot delete an active project status");
                    var deleteIconColor = (row.isActive == false ? "#E34724" : "gray");
                    var deleteAction = (row.isActive == false ? `deleteRecord('Project Track - Project Status', 'Are you sure you want to delete the project status?', '/Admin/ProjectStatus/Delete?id=${data}');` : "");

                    return `<div class="text-center">
                                <a href="${editAction}" style="color: ${editIconColor}; text-decoration: none;" title="${editTitle}">
                                    <i class="fas fa-pen"></i>
                                </a>
                                &nbsp;
                                <a style="color: ${deleteIconColor}; cursor:pointer;" title="${deleteTitle}" onclick="${deleteAction}">
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
        width: "100%"
    });
}