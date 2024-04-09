
counter = 0

# Cooperate = 0 & Defect = 1

def NextIteration():
    global counter

    if counter == 0:
        counter += 1
        return 0
    else:
        counter = 0
        return 1

def Reset():
    pass

def SetResult(a, b):
    pass
