import { DeleteProduct, UpdateMealSummary, EditIcon } from './UpdateMealSummaryAndDeleteProduct.js';
$(document).ready(function () {
    $.ajax({
        url: '/Meal/GetSelectedProducts',
        dataType: 'json',
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                var rowId = result[i].productId + result[i].grams;
                var row = $('<tr id =' + rowId + ' > ');
                row.append('<td>' + result[i].productName + '</td>');
                row.append('<td>' + result[i].kcal.toFixed(1) + '</td>');
                row.append('<td>' + result[i].protein.toFixed(1) + '</td>');
                row.append('<td>' + result[i].carbs.toFixed(1) + '</td>');
                row.append('<td>' + result[i].fat.toFixed(1) + '</td>');
                row.append('<td>' + result[i].grams.toFixed(1) + '</td>');

                var jsonData = JSON.stringify(result[i]);

                var $iconLink = $('<a>', {
                    href: 'javascript:void(0)',
                    class: 'icon-link',
                    'data-product-attributes': jsonData
                }).html('<i class="bi bi-trash3-fill icon-size"></i>');
                var $EditIconLink = $('<a>', {
                    href: 'javascript:void(0)',
                    class: 'icon-link',
                    'data-product-attributes': jsonData
                }).html('<i class="bi bi-pencil-square icon-size"></i>');

                var $td = $('<td>').append($iconLink);
                var $td1 = $('<td>').append($EditIconLink);

                $(row).append($td1);
                $(row).append($td);

                EditIcon($EditIconLink);
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