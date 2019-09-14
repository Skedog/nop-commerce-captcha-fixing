# Fixes for NOPCommerce Captcha
I spent entirely too long trying to figure out how to fix this problem. Thanks to [this post](https://www.nopcommerce.com/boards/t/33692/support-for-new-version-recaptcha-v2.aspx#139455) I was able to republish a version of NOP Commerce that had the captcha fixed.
### Quick Fix
- Download the two files
  - Nop.Web.Framework.dll
  - Recaptcha.Web.dll
- Put those two files in your production /bin folder
- Fixed


## If you are looking to compile the fix yourself, the steps are:
### Self-Compile
  - Download Visual Studio
  - Download version 3.6 of NOP Commerce
  - Open the NOP Commerce soultion(project) in VS
  - In the right column - expand "Presentatiom -> Nop.Web.Framework"
  - Right click "References" and click "Manage Nuget Packages"
  - In the search - look for "reCAPTCHA4net" and add that to your solution
    - Back in the "References" dialog, delete the existing "Recaptcha" reference
  - In Nop.Web.Framework > Security > Captcha open "HtmlExtensions.cs" and "CaptchaValidatorAttribute.cs" and put the code in the "source" folder above.
  - Right click Nop.Web and click "Build" and then right click and select "Publish"
  - Copy the following files from your "publish" directory -> /bin and place them in your production /bin folder
    - Nop.Web.Framework.dll
    - Recaptcha.Web.dll
