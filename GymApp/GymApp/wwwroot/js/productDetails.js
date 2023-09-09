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
            var row = $('<tr>');
            row.append('<td>' + productAttributes.productName + '</td>');
            row.append('<td>' + productAttributes.kcal + '</td>');
            row.append('<td>' + productAttributes.protein + '</td>');
            row.append('<td>' + productAttributes.carbs + '</td>');
            row.append('<td>' + productAttributes.fat + '</td>');
            row.append('<td>' + productAttributes.grams + '</td>');
            $('#ProductTable').append(row);


            $.ajax({
                url: '/Meal/UpdateSummary',
                dataType: 'json',
                success: function (updatedSummary) {
                    updateMealSummary(updatedSummary)
                },
                error: function (innerError) {
                    console.error('Inner request error:', innerError);
                }
            });
        },
    });


    
    function updateMealSummary(updatedSummary)
    {
        $('#TotalKcal').text(updatedSummary.totalKcal);
        $('#TotalProtein').text(updatedSummary.totalProtein);
        $('#TotalCarbs').text(updatedSummary.totalCarbs);
        $('#TotalFat').text(updatedSummary.totalFat);
        $('#TotalGrams').text(updatedSummary.totalGrams);
    }
});
