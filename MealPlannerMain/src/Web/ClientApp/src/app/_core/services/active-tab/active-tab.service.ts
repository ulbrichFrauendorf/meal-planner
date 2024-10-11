import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class ActiveTabService {
    private activeTabIndex = 0;

    setActiveTabIndex(index: number) {
        this.activeTabIndex = index;
    }

    getActiveTabIndex(): number {
        return this.activeTabIndex;
    }
}
