import { Tab, Tabs } from "@mui/material";
import React, { SyntheticEvent } from "react";

interface IProps {
  selectedTab: number;
  setSelectedTab: (tab: number) => void;
}

const PostFilter = ({ selectedTab, setSelectedTab }: IProps) => {
  const handleChange = (event: SyntheticEvent, newValue: number) => {
    setSelectedTab(newValue);
  };

  return (
    <>
      <Tabs value={selectedTab} onChange={handleChange}>
        <Tab label="My Posts" />
        <Tab label="All Posts" />
      </Tabs>
    </>
  );
};

export default PostFilter;
