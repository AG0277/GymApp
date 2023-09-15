import { DeleteProduct, UpdateMealSummary, EditProduct } from './UpdateMealSummaryAndDeleteProduct.js';

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
            row.append('<td>' + productAttributes.kcal + '</td>');
            row.append('<td>' + productAttributes.protein + '</td>');
            row.append('<td>' + productAttributes.carbs + '</td>');
            row.append('<td>' + productAttributes.fat + '</td>');
            row.append('<td>' + productAttributes.grams + '</td>');

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

            $EditIconLink.on('click', function () {
                var row = document.getElementById(rowId);

                var fifthTd = $(row).find('td:eq(5)');
                var currentText = fifthTd.text();
                var columnWidth = '10%';

                fifthTd.css('width', columnWidth);

                var inputElement = $('<input>', {
                    type: 'text',
                    value: currentText,
                    style: 'width:100%'
                });

                fifthTd.html(inputElement);
                inputElement.focus();
                inputElement.blur(function () {
                    var newValue = inputElement.val();

                    fifthTd.text(newValue);
                });
            });
            $('#ProductTable').append(row);
            UpdateMealSummary();
        },
    });
});
