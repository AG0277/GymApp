$(document).ready(function () {
    $.ajax({
        url: '/Meal/GetSelectedProducts',
        dataType: 'json',
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                var rowId = result[i].productId + result[i].grams;
                var row = $('<tr id =' + rowId + ' > ');
                row.append('<td>' + result[i].productName + '</td>');
                row.append('<td>' + result[i].kcal + '</td>');
                row.append('<td>' + result[i].protein + '</td>');
                row.append('<td>' + result[i].carbs + '</td>');
                row.append('<td>' + result[i].fat + '</td>');
                row.append('<td>' + result[i].grams + '</td>');

                var jsonData = JSON.stringify(result[i]);

                var $iconLink = $('<a>', {
                    href: 'javascript:void(0)',
                    class: 'icon-link',
                    'data-product-attributes': jsonData
                }).html('<i class="bi bi-trash3-fill icon-size"></i>');

                var $td = $('<td>').append($iconLink);
                $(row).append($td);

                (function (currentJsonData, currentRowId) {
                    $iconLink.on('click', function () {
                        $.ajax({
                            type: "POST",
                            url: '/Meal/DeleteProduct?jsonData=' + currentJsonData,
                            data: { jsonData: currentJsonData },
                            dataType: 'json',
                            success: function () {
                                document.getElementById(currentRowId).remove();
                                UpdateMealSummary();
                            },
                        });
                    });
                })(jsonData, rowId); 

                $('#ProductTable').append(row);
                UpdateMealSummary();
            }
        },
    });
    function UpdateMealSummary() {
        $.ajax({
            url: '/Meal/UpdateSummary',
            dataType: 'json',
            success: function (updatedSummary) {
                $('#TotalKcal').text(updatedSummary.totalKcal);
                $('#TotalProtein').text(updatedSummary.totalProtein);
                $('#TotalCarbs').text(updatedSummary.totalCarbs);
                $('#TotalFat').text(updatedSummary.totalFat);
                $('#TotalGrams').text(updatedSummary.totalGrams);
            },
            error: function (innerError) {
                console.error('Inner request error:', innerError);
            }
        });
    }
});