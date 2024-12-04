import tweepy
import sys

# Configura tus credenciales de acceso a la API de Twitter
api_key = '5Clsss7jlyWS9k1g3Cm48bA79'
api_key_secret = 'FHFfhLAQvDmzgt0v8el1muxLaYyccVFSNOAPMIDR6N6aXlb3lY'
access_token = '1863097261497933824-elj4rzZw1602bpnLfzDDMcTYaxblaW'
access_token_secret = 'pnC24gX0MULBLSeq2sLRfbXfbNDvZSSYRk0mO1sr9jSGK'
bearer_token = 'AAAAAAAAAAAAAAAAAAAAACEnxQEAAAAACOn76s%2FQ4wwt16uEE3jbhwFR%2Bxk%3DvHzshcpH7MSxvkN7HBar3p6ZlR6GAa1Aysp8z0CiNi0ZAdvpEv'

# Autenticación con Twitter utilizando OAuth 2.0
client = tweepy.Client(bearer_token=bearer_token, 
                       consumer_key=api_key, 
                       consumer_secret=api_key_secret, 
                       access_token=access_token, 
                       access_token_secret=access_token_secret)

# Texto del tweet pasado como argumento
tweet_text = sys.argv[1]

# Publicar el tweet
try:
    response = client.create_tweet(text=tweet_text)
    print("Tweet publicado con éxito! ID del Tweet:", response.data['id'])
except tweepy.TweepyException as e:
    print("Error al publicar el tweet: ", e)
