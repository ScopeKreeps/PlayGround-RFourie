using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twitterizer;

namespace TwitterSandBox
{
    public partial class TwitterConnect : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            var oauth_consumer_key = "fxZy0SjfOSmMSfbpnKdQw0h2D";
            var oauth_consumer_secret = "PGV7BnV13VU3Kd1V24IpDHzzX3CVMNvLa1AO8ZqO3gDkRbJTIh";

            if (Request["oauth_token"] == null)
            {
                OAuthTokenResponse reqToken = OAuthUtility.GetRequestToken(oauth_consumer_key, 
                                                                           oauth_consumer_secret, 
                                                                           Request.Url.AbsoluteUri);

                Response.Redirect(string.Format("http://twitter.com/oauth/authorize?oauth_token={0}", reqToken.Token));
            }
            else
            {
                string requestToken = Request["oauth_token"].ToString();
                string pin = Request["oauth_verifier"].ToString();

                var tokens = OAuthUtility.GetAccessToken(oauth_consumer_key,
                                                        oauth_consumer_secret,
                                                        requestToken,
                                                        pin);

                OAuthTokens accesstoken = new OAuthTokens()
                {
                    AccessToken = tokens.Token,
                    AccessTokenSecret = tokens.TokenSecret,
                    ConsumerKey = oauth_consumer_key,
                    ConsumerSecret = oauth_consumer_secret
                };

                TwitterResponse<TwitterStatus> response = TwitterStatus.Update
                    (accesstoken, "This should be posted as a tweet");

                if (response.Result == RequestResult.Success)
                {
                    Response.Write("Success");
                }
                else
                {
                    Response.Write("UnSuccessfull");
                }
            }
        }
    }
}