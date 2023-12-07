import speech_recognition as sr
import os
import openai
import pygame
from google.cloud import texttospeech_v1beta1 as texttospeech
import time
import threading

os.environ['GOOGLE_APPLICATION_CREDENTIALS'] = r"C:\Users\Spher\OneDrive\Desktop\CS\AI\Kuebiko\AI-Virtual_Streamer\polynomial-land-400201-c0247bbd8ec8.json"

openai.api_key = "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"
openai.api_base = 'https://api.openai.com/v1/chat'

conversation = [
    
]

def open_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as infile:
        return infile.read()
    
conversation.append({ 'role': 'system', 'content': open_file(r"C:\Users\Spher\OneDrive\Desktop\CS\AI\Kuebiko\AI-Virtual_Streamer\prompt_chat.txt") })

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

def captions(mark_array, response):
    count = 0
    current = 0
    for i in range(len(response.timepoints)):
        count += 1
        current += 1
        with open("output.txt", "a", encoding="utf-8") as out:
            out.write(mark_array[int(response.timepoints[i].mark_name)] + " ")
        if i != len(response.timepoints) - 1:
            total_time = response.timepoints[i + 1].time_seconds
            time.sleep(total_time - response.timepoints[i].time_seconds)
        if current == 25:
                open('output.txt', 'w', encoding="utf-8").close()
                current = 0
                count = 0
        elif count % 7 == 0:
            with open("output.txt", "a", encoding="utf-8") as out:
                out.write("\n")
    time.sleep(2)
    open('output.txt', 'w').close()

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
    time.sleep(2)
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
    
    print(f"Time taken to transcribe audio {time.time() - start}")

    with open(file_path, 'a') as file3:
        file3.write(f"Time taken to transcribe audio {time.time() - start}")

    start2 = time.time()
    response = gpt3_completion(conversation)
    # print(f"GPT-3 Response: {response}")

    conversation.append({"role": "assistant", "content": response})

    with open(file_path, 'a') as file3:
        file3.write(f"GPT said {response}\n")

    with open(file_path, 'a') as file3:
        file3.write(f"Time taken for GPT to generate response {time.time() - start2}\n")
    
    print(f"Time taken for GPT to generate response {time.time() - start2}\n")

    start3 = time.time()

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

    with open(file_path, 'a') as file3:
        file3.write(f"Time taken to turn text into audio file: {time.time() - start3}\n")

    print(f"Time taken to turn text into audio file: {time.time() - start3}\n")

    # The response's audio_content is binary.
    start4 = time.time()

    time_thread = threading.Thread(target=text_to_speech_and_play, args=(response.audio_content,))
    time2_thread = threading.Thread(target=captions, args=(mark_array, response))
    

    time2_thread.start()
    time_thread.start()
    
    
    time_thread.join()
    time2_thread.join()
    


    
    with open(file_path, 'a') as file3:
        file3.write(f"Time to speak audio: {time.time() - start4}\n")

    print(f"Time to speak audio: {time.time() - start4}\n")

    with open(file_path, 'a') as file3:
        file3.write(f'The entire process took: {time.time() - start} seconds \n')
    
    print(f'The entire process took: {time.time() - start} seconds \n')

    # with open(file_path, 'r') as file2:
    #     print(file2.read())




