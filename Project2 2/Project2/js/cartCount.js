$(document).ready(function()
{
    var count = 0;

    $('.cart-btn').click(function()
    {
        count = count+1;
        $('.count').html(count);
});
});