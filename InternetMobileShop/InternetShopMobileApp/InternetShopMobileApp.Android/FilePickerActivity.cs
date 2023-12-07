using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Net;
using System;
using InternetShopMobileApp.Views;

namespace InternetShopMobileApp.Android;

[Activity(Label = "FilePickerActivity")]
public class FilePickerActivity : Activity
{
    private static FilePickerActivity instance;

    public static FilePickerActivity getInstance()
    {
        if (instance == null)
            instance = new FilePickerActivity();
        return instance;
    }
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        instance = this;

        // Create your application here
    }

    public static readonly int PickImageId = 1000;
    private ImageView _imageView;

    public void ButtonOnClick(object sender, EventArgs eventArgs)
    {
        Intent = new Intent();
        Intent.SetType("image/*");
        Intent.SetAction(Intent.ActionGetContent);
        StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
    }
    // Create a Method OnActivityResult(it is select the image controller)   
    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
        {
            //Android.Net.Uri uri = data.Data;
            //_imageView.SetImageURI(uri);
        }
    }
}