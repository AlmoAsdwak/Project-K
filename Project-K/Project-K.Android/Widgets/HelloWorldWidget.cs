using Android.Appwidget;
using Android.Content;
using Android.Widget;

namespace Kyberna.Widgets
{
    [BroadcastReceiver(Label = "Rozvrh Widget")]
    public class HelloWorldWidget : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(HelloWorldWidget)).Name);
            var views = BuildRemoteViews(context);
            appWidgetManager.UpdateAppWidget(me, views);
        }

        private RemoteViews BuildRemoteViews(Context context)
        {
            var views = new RemoteViews(context.PackageName, Resource.Layout.widget);
            views.SetTextViewText(Resource.Id.widget_textview, "Hello World");
            return views;
        }


    }

}