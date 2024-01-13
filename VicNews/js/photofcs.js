$=function (a){
	return document.getElementById(a);
}

//½¹µãÍ¼Æ¬ÇÐ»»
function focus_change()
{
	$("focus_img"+focus_now).style.display="none";
	$("focus_button"+focus_now).className="b1";
	focus_now++;
	if(focus_now>focus_max-1)
		focus_now=0;
	$("focus_title").innerHTML=titles[focus_now];
	$("focus_img"+focus_now).style.display="block";
	$("focus_button"+focus_now).className="b2";
}

function focus_change1(a)
{
	$("focus_img"+focus_now).style.display="none";
	$("focus_button"+focus_now).className="b1";
	focus_now=a;
	$("focus_title").innerHTML=titles[focus_now];
	$("focus_img"+focus_now).style.display="block";
	$("focus_button"+focus_now).className="b2";
}
