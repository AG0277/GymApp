import { UpdateEditMealSummary, EditEditIcon } from './UpdateMealSummaryAndDeleteProduct.js';
$(document).ready(function () {


    var iElements = document.querySelectorAll('#ProductTable i[data-product]');

    iElements.forEach(function (element) {
        var jsonData = JSON.parse(element.getAttribute('data-product'));
        console.log(jsonData);

        var jsonObject = {
            ProductId: jsonData.productId,
            ProductName: jsonData.productName,
            kcal: jsonData.kcal,
            protein: jsonData.protein,
            carbs: jsonData.carbs,
            fat: jsonData.fat,
            grams: jsonData.grams,
            MealProducts: null
        };

        var jsonString = JSON.stringify(jsonObject);
        var row = element.closest('tr');
        var sixthTd = $(row).find('td:eq(6)');
        var $iconLink = $('<a>', {
            href: 'javascript:void(0)',
            class: 'icon-link',
            'data-product-attributes': jsonString
        }).html('<i class="bi bi-pencil-square icon-size"></i>');
        sixthTd.html($iconLink);
        EditEditIcon($iconLink);
    });
});