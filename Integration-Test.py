from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support.ui import Select
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC


def wait_set_account(id_, value):
    # Wait until dropdown id_ is available and set it to given label
    element = WebDriverWait(driver, 10).until(
           EC.presence_of_element_located((By.ID, id_)))
    account = Select(element)
    account.select_by_visible_text(value)


def check_withdraw():
    # Withdraw some money from account, checking balance before/after.
    wait_set_account("BalanceAccount", "Mat")
    driver.find_element_by_id("ShowBalance").click()
    element = WebDriverWait(driver, 10).until(
           EC.presence_of_element_located((By.ID, "Balance")))
    before_withdraw = int(element.text[0:-3])
    driver.back()
    
    wait_set_account( "WithdrawAccount", "Mat")
    element = driver.find_element_by_id("WithdrawAmount")
    element.send_keys("200");
    driver.find_element_by_id("Withdraw").click()
    driver.back();
    
    wait_set_account( "BalanceAccount", "Mat")
    driver.find_element_by_id("ShowBalance").click()
    element = WebDriverWait(driver, 10).until(
           EC.presence_of_element_located((By.ID, "Balance")))
    assert int(element.text[0:-3]) == before_withdraw - 200
    driver.back()


def check_list_accounts():
    # check that list accounts is sane.
    driver.find_element_by_id("ListAccounts").click()
    assert "List Accounts" in driver.title
    bodyText = driver.find_element_by_tag_name('body').text
    assert "Lönekonto" in bodyText
    driver.back()


# Select user 0 Orvar Slusk
driver = webdriver.Firefox()
driver.get("http://localhost:59959/");
assert "Home Page" in driver.title
dropdown = Select(driver.find_element_by_id("UserName"))
dropdown.select_by_visible_text("Orvar Slusk");
driver.find_element_by_id("SelectUser").click()

check_list_accounts()
check_withdraw()
