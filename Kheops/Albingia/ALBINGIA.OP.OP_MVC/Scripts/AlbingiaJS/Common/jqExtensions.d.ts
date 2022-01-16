interface JQuery<TElement = HTMLElement> {
    /**
    * disable the set of matched elements.
    * @param addClass defines whether a disable class has to be assigned to matched elements. Class name is "readonly"
    * */
    disable(addClass?: boolean): this;
    /**
     * 
     * @param addClass defines whether a readonly class has to be assigned to matched elements. Class name is "readonly"
     */
    makeReadonly(addClass?: boolean): this;
    /**
     * @returns whether the selection has at least one element
     * */
    exists(): boolean;
    /**
     * disable the set of matched elements.
     * */
    enable(): this;
    /**
     * check the set of matched elements.
     * @param silent whether not triggering change
     * */
    check(silent: boolean): this;
    /**
     * uncheck the set of matched elements.
     * @param silent whether not triggering change
     * */
    uncheck(silent: boolean): this;
    /**
     * add invalid input class to each matched element of the set.
     * */
    setInvalid(): this;
    /**
     * add selected attribute to each matched element of the set.
     * */
    makeSelected(): this;
    /**
     * remove selected attribute from each matched element of the set.
     * */
    unselect(): this;
    /**
     * set a value from each matched element (select or radio) and trigger change
     * @param value 
     * */
    selectVal(value: any): this;
    /**
     * equivalent to JQuery(document).off(events, this.selector).on(events, this.selector, handler)
     */
    offOn<TType extends string, TData>(
        events: TType,
        data: TData,
        handler: any
    ): this;
    /**
    * equivalent to offOn('click', handler)
    */
    kclick<TType extends string>(
        events: TType,
        handler: any
    ): this;
    /**
    * equivalent to trigger('click') if the selectee is not disabled, else does nothing
    */
    triggerClick(
    ): this;
    /**
     * set the value of matched elements to empty.
     * */
    clear(): this;
    /**
     * empty the inner html of each element in the selection
     * */
    clearHtml(): this;
    /**
     * @returns whether the elemen is visble
     * */
    isVisible(): boolean;
    /**
     * @returns whether the element checked
     * */
    isChecked(): boolean;
    /**
     * defines whether the matched elements values exist and are not empty
     * @param considerSpaces whether the left and right spaces are considered has not empty
     * @returns whether the element value exists and is not empty
     * */
    hasVal(considerSpaces?: boolean): boolean;

    /**
     * defines whether the matched elements values are empty text or NULL
     * @param considerSpaces defines whether the left and right spaces are considered has not empty
     */
    isEmpty(considerSpaces?: boolean): boolean;

    /**
     * defines whether val is "true" meaning like 1, "1" or "true"
     * */
    hasTrueVal(): boolean;

    jqDialogErreur(div: string, exception: Error): void;
    jqDialogErreur(exception: Error): void;

    /** 
     * serializes inputs
     * @returns a JS object 
     * */
    serializeObject(ignoreDisabled: boolean, onCreate: void): { [key: string]: string };

    /**
     * assign an accesskey to the element through accesskey attribute or any custom acceskey attribute
     * @param keyChar
     */
    assignAccessKey(keyChar: string, attr?: string): this;
}