$(document).ready(function () {
    //Initialises DataTable
    $('#usersTable').DataTable({
        order: [[1, 'asc']],
        "columns": [
            null,
            { "searchable": false },
            { "searchable": false },
            { "searchable": false }
        ]
    });


});


//function for updating the role of the selected user from the DataTable
function updateRole(email, role) {
    fetch('/Management/API/User/UpdateUser', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ email: email, role: role })
    })
        .then(response => {
            if (response.ok) {
                const roleElement = document.getElementById(`role-${email}`);
                roleElement.textContent = role;
            }
        })
        .catch(error => {
            alert("Unable to update user role, please contact an administrator");
        });
}




