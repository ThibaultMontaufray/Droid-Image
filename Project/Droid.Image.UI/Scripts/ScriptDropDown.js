$(document).ready(function() {

  // Default dropdown action to show/hide dropdown content
  $('.js-dropp-action').click(function(e) {
    e.preventDefault();
    $(this).toggleClass('js-open');
    $(this).parent().next('.dropp-body').toggleClass('js-open');
  });

  // Using as fake input select dropdown
  $('label').click(function() {
    $(this).addClass('js-open').siblings().removeClass('js-open');
    $('.dropp-body,.js-dropp-action').removeClass('js-open');
  });
  // get the value of checked input radio and display as dropp title
  $('input[name="dropp"]').change(function() {
    var value = $("input[name='dropp']:checked").val();
    $('.js-value').text(value);
  });

});

var _gaq = _gaq || [];
_gaq.push(['_setAccount', 'UA-36251023-1']);
_gaq.push(['_setDomainName', 'jqueryscript.net']);
_gaq.push(['_trackPageview']);

(function() {
	var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
	ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
	var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();

updateDropDown = function(item)
{
    var value = $("input[name='dropp-" + item +"']:checked").val();
    $('.js-value-' + item).text(value);
}