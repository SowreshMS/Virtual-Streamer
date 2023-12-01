import speech_recognition as sr
import os
import openai
import pygame
from google.cloud import texttospeech_v1beta1 as texttospeech
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

# Function to generate and play audio from binary audio data
def text_to_speech_and_play(audio_content):
    # Save the audio content to a file
    with open("output.mp3", "wb") as out:
        out.write(audio_content)

    # Initialize pygame mixer
    pygame.mixer.init()

    # Load and play the audio file
    pygame.mixer.music.load("output.mp3")
    pygame.mixer.music.play()

    # Wait for the audio to finish playing
    while pygame.mixer.music.get_busy():
        pygame.time.Clock().tick(10)

    # Quit the mixer
    pygame.mixer.quit()

recognizer = sr.Recognizer()

file_path = os.path.join(os.path.dirname(__file__), 'logs.txt')

# file = open(file_path, 'a')

while True:
    start = time.time()
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
            text = "Say \"Could not request results from Google Speech Recognition service\""

    with open(file_path, 'a') as file3:
        file3.write(f"You said: {text} \n")

    conversation.append({"role": "user", "content": text})

    response = gpt3_completion(conversation)
    # print(f"GPT-3 Response: {response}")

    conversation.append({"role": "assistant", "content": response})

    with open(file_path, 'a') as file3:
        file3.write(f"GPT said {response}\n")

    client = texttospeech.TextToSpeechClient()

    ssml_text = f'<speak>{response}</speak>'

    input_text = texttospeech.SynthesisInput(ssml=ssml_text)

    voice = texttospeech.VoiceSelectionParams(
        language_code="en-US",
        name="en-US-Neural2-F",
        ssml_gender=texttospeech.SsmlVoiceGender.FEMALE,
    )

    audio_config = texttospeech.AudioConfig(
        audio_encoding=texttospeech.AudioEncoding.MP3,
    )

    response = client.synthesize_speech(
        request={"input": input_text, "voice": voice, "audio_config": audio_config, "enable_time_pointing": ["SSML_MARK"]}
    )

    # The response's audio_content is binary.
    text_to_speech_and_play(response.audio_content)

    with open(file_path, 'a') as file3:
        file3.write(f'The entire process took: {time.time() - start} seconds \n')

    with open(file_path, 'r') as file2:
        print(file2.read())

