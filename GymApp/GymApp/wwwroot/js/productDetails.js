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
            var row = $('<tr id =' + productAttributes.productId + productAttributes.totalGrams+' > ');
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

            var $td = $('<td>').append($iconLink);
            $(row).append($td);
            $iconLink.on('click', function () {
                $.ajax({
                    type: "POST",
                    url: '/Meal/DeleteProduct?jsonData=' + jsonData,
                    data: { jsonData: jsonData },
                    dataType: 'json',
                    success: function () {
                        document.getElementById(productAttributes.productId + productAttributes.totalGrams).remove();
                        UpdateMealSummary();
                    },
                });
            });
            $('#ProductTable').append(row);
            UpdateMealSummary();
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
