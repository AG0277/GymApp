import { DeleteProduct, UpdateMealSummary } from './UpdateMealSummaryAndDeleteProduct.js';
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
                        DeleteProduct(currentJsonData, currentRowId)
                            .then(function () {
                                UpdateMealSummary();
                            })
                    });
                })(jsonData, rowId); 

                $('#ProductTable').append(row);
                UpdateMealSummary();
            }
        },
    });
});