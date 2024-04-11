using Android.Appwidget;
using Android.Content;
using Android.Widget;

namespace Kyberna.Widgets
{
    [BroadcastReceiver]
    public class HelloWorldWidget : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            RemoteViews remoteViews = new RemoteViews(context.PackageName, Resource.Layout.widget);
            ComponentName thisWidget = new ComponentName(context, Java.Lang.Class.FromType(typeof(HelloWorldWidget)).Name);
            remoteViews.SetTextViewText(Resource.Id.Subject, "test");
            AppWidgetManager.GetInstance(context).UpdateAppWidget(thisWidget, remoteViews);
        }

    }
}