$(document).on('click', '#searchResults a', function (e) {
    e.preventDefault();
    var productId = $(this).data('productid');

    $.ajax({
        url: '/Meal/GetProductAttributes?productId=' + productId,
        data: { productId: productId },
        dataType: 'json',
        success: function (productAttributes) {
            var productAttributesDiv = $('#productAttributes');
            productAttributesDiv.empty();

            var attributesList = $('<ul>');
            attributesList.append('<li>Name: ' + productAttributes.productName + '</li>');
            attributesList.append('<li>Kcal: ' + productAttributes.kcal + '</li>');
            attributesList.append('<li>Protein: ' + productAttributes.protein + '</li>');
            attributesList.append('<li>Carbs: ' + productAttributes.carbs + '</li>');
            attributesList.append('<li>Fat: ' + productAttributes.fat + '</li>');
            attributesList.append('<li>Grams: ' + productAttributes.grams + '</li>');
            // Add more attributes as needed

            productAttributesDiv.append(attributesList);
        },
    });
});
