git rev-list --objects --all | cut -d ' ' -f2 | grep . | sort | uniq -c | sort -rn