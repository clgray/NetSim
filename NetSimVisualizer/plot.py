from matplotlib import pyplot as plt
import numpy as np


log_dir_path = "./Logs"
log_tag = "1c31102e-2b4e-4cca-b4c3-b51c599ec245"

files = [
    {"filename": f"Netsim-Node-Metrics-queue-{log_tag}", "title": "Node: Messages in queue average"},
    {"filename": f"Netsim-Node-Metrics-load-{log_tag}", "title": "Node: Load average"},
    {"filename": f"Netsim-Connection-Metrics-queue-{log_tag}", "title": "Connection: Messages in queue average"},
    {"filename": f"Netsim-Connection-Metrics-load-{log_tag}", "title": "Connection: Load average"},
    {"filename": f"Netsim-Message-Metrics-failed-{log_tag}", "title": "Message: failed"},
]


def load_csv_file(path):
    file = open(path)
    data = file.readlines()
    splitted_data = [x.split(',') for x in data]
    return splitted_data, len(splitted_data[0])


def aggeregate_line(array):
    aggregated = [sum([float(y)/len(x) for y in x]) for x in array]
    return aggregated, len(aggregated)


def plot(x, y, title):
    fig, ax = plt.subplots()
    ax.plot(x, y, label=title, c="blue")
    ax.set_title(title)


for file in files:
    data, line_len = load_csv_file(f"{log_dir_path}/{file['filename']}")
    aggregated, count = aggeregate_line(data)
    plot(range(0, count), aggregated, file['title'])

plt.show()