$(document).ready(function () {
    $.ajax({
        url: '/Meal/GetSelectedProducts',
        dataType: 'json',
        success: function (result) {
            for (var i = 0; i < result.length;i++ ) {
                var row = $('<tr>');
                row.append('<td>' + result[i].productName + '</td>');
                row.append('<td>' + result[i].kcal + '</td>');
                row.append('<td>' + result[i].protein + '</td>');
                row.append('<td>' + result[i].carbs + '</td>');
                row.append('<td>' + result[i].fat + '</td>');
                row.append('<td>' + result[i].grams + '</td>');
                $('#ProductTable').append(row);
            }
        },
    });
});