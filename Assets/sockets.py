import speech_recognition as sr
import sounddevice as sd
import wave
import os
import openai
import pygame
from google.cloud import texttospeech_v1beta1 as texttospeech
from gtts import gTTS
import time


os.environ['GOOGLE_APPLICATION_CREDENTIALS'] = r"C:\Users\Spher\OneDrive\Desktop\CS\AI\Kuebiko\AI-Virtual_Streamer\polynomial-land-400201-c0247bbd8ec8.json"

openai.api_key = "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"
openai.api_base = 'https://api.openai.com/v1/chat'

conversation = [
    {"role": "system", "content": "You are a helpful assistant."}
  ]

def gpt3_completion(messages, engine='gpt-3.5-turbo', temp=0.9, tokens=100, freq_pen=2.0, pres_pen=2.0, stop=['DOGGIEBRO:', 'CHATTER:']):
    response = openai.Completion.create(
        model=engine,
        messages=messages,
        temperature=temp,
        max_tokens=tokens,
        frequency_penalty=freq_pen,
        presence_penalty=pres_pen,
        stop=stop)
    text = response['choices'][0]['message']['content'].strip()
    return text

# Function to generate and play audio from text
def text_to_speech_and_play(text, language='en', filename='output.mp3'):
    # Generate the audio file
    # tts = gTTS(text=text, lang=language)
    # tts.save(filename)

    # Initialize pygame mixer
    pygame.mixer.init()

    # Load and play the audio file
    pygame.mixer.music.load(filename)
    pygame.mixer.music.play()

    # Wait for the audio to finish playing
    while pygame.mixer.music.get_busy():
        pygame.time.Clock().tick(10)

    print("audio")
    
    # Quit the mixer
    pygame.mixer.quit()

recognizer = sr.Recognizer()

while True:
    # Record audio from the microphone
    with sr.Microphone() as source:
        print("Say something:")
        audio = recognizer.listen(source)

        try:
            text = recognizer.recognize_google(audio)
            print(f"You said: {text}")

        except sr.UnknownValueError:
            print("Speech Recognition could not understand audio")
            text = "Say \"I am sorry but I was not able to understand you\""
        except sr.RequestError as e:
            print(f"Could not request results from Google Speech Recognition service; {e}")
            text = "Say \"I am sorry but I was not able to understand you\""

    start = time.time()
    with open(os.path.join(os.path.dirname(__file__), 'logs.txt'), 'w') as file:
        file.write(text)
        file.write(f'\n{time.time() - start} \n')

    conversation.append({"role": "user", "content": text})

    response = gpt3_completion(conversation)

    conversation.append({"role": "assistant", "content": response})

    print(response)

    client = texttospeech.TextToSpeechClient()

    # response = message.content + "? " + response
    ssml_text = '<speak>'
    response_counter = 0
    mark_array = []
    for s in response.split(' '):
        ssml_text += f'<mark name="{response_counter}"/>{s}'
        mark_array.append(s)
        response_counter += 1
    ssml_text += '</speak>'

    input_text = texttospeech.SynthesisInput(ssml = ssml_text)

    # Note: the voice can also be specified by name.
    # Names of voices can be retrieved with client.list_voices().
    voice = texttospeech.VoiceSelectionParams(
        language_code="en-US",
        name= "en-US-Neural2-F",
        ssml_gender=texttospeech.SsmlVoiceGender.FEMALE,
    )

    audio_config = texttospeech.AudioConfig(    
        audio_encoding=texttospeech.AudioEncoding.MP3,
    )


    response = client.synthesize_speech(
        request={"input": input_text, "voice": voice, "audio_config": audio_config, "enable_time_pointing": ["SSML_MARK"]}
    )

    print("hi")
    # The response's audio_content is binary.
    with open("output.mp3", "wb") as out:
        out.write(response.audio_content)


    audio_file = os.path.join(os.path.dirname(__file__), 'output.mp3')


    print(audio_file)

    # Example usage
    text_to_speech_and_play(response)




