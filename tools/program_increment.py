import subprocess

# Checkout the main branch.
subprocess.check_call(['git', 'checkout', 'main'])

print("Make sure you don't have any files staged before running this script.\n")

# Get the branch name from the user.
branch_name = input('Enter the name of the new branch, following the convention year/pi#: ')

# Checkout a new branch based on main.
subprocess.check_call(['git', 'checkout', '-b', branch_name])

# Generate the subset of commits that belong to v.next but not main.
commits = subprocess.check_output(['git', 'log', '--pretty=format:%H', 'v.next', '^main']).decode('utf-8').split('\n')

# Iterate through the commit subset.
ignore_case = "[Automated] Sync v.next with main"
print("Cherry-picking commits:\n")
for commit in commits:

    # Store the commit message.
    message = subprocess.check_output(['git', 'log', '-1', '--pretty=format:%s', commit]).decode('utf-8')
    
    # If the commit message contains the ignore case string, skip it.
    if ignore_case in message:
        continue

    # Cherry-pick the commit to the new branch.
    try:
        subprocess.check_call(['git', 'cherry-pick', commit])
    except subprocess.CalledProcessError:
        subprocess.check_call(['git', 'cherry-pick', '--quit'])