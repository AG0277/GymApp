import { DeleteProduct, UpdateMealSummary, EditProduct, EditIcon } from './UpdateMealSummaryAndDeleteProduct.js';

$(document).on('click', '#searchResults a', function (e) {
    e.preventDefault();
    var productId = $(this).data('productid');

    $.ajax({
        url: '/Meal/AddProductsToMeal?productId=' + productId,
        data: { productId: productId },
        dataType: 'json',
        success: function (productAttributes) {
            $('#searchInput').val('Search products...')
            $('#searchInput').on('focus', function () {
                if ($(this).val() === 'Search products...') {
                    $(this).val('');
                }
            });
            var rowId = productAttributes.productId + productAttributes.totalGrams;
            var row = $('<tr id =' + rowId + ' > ');
            row.append('<td>' + productAttributes.productName + '</td>');
            row.append('<td>' + productAttributes.kcal.toFixed(1) + '</td>');
            row.append('<td>' + productAttributes.protein.toFixed(1) + '</td>');
            row.append('<td>' + productAttributes.carbs.toFixed(1) + '</td>');
            row.append('<td>' + productAttributes.fat.toFixed(1) + '</td>');
            row.append('<td>' + productAttributes.grams.toFixed(1) + '</td>');

            var jsonData = JSON.stringify(productAttributes);

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

            $iconLink.on('click', function () {
                DeleteProduct(jsonData, rowId).then(function () {
                    UpdateMealSummary();
                })
            });
            EditIcon($EditIconLink);
            $('#ProductTable').append(row);
            UpdateMealSummary();
        },
    });
});
